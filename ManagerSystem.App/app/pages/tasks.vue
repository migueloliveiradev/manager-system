<script setup lang="ts">
import type { BaseResponse, Project, TaskColumn, User, WorkTask, TaskHistory, Comment } from '~/types/api'

const route = useRoute()
const api = useApi()

const projects = ref<Project[]>([])
const users = ref<User[]>([])
const columns = ref<TaskColumn[]>([])
const tasks = ref<WorkTask[]>([])
const selectedTask = ref<WorkTask | null>(null)
const taskHistory = ref<TaskHistory[]>([])
const comments = ref<Comment[]>([])
const commentText = ref('')
const loading = ref(false)

const filters = reactive({
  projectId: String(route.query.projectId || ''),
  assigneeId: '',
  priority: 0,
  statusId: '',
  dueDateUntil: ''
})

const taskModalOpen = ref(false)
const detailsModalOpen = ref(false)
const editingTask = ref<WorkTask | null>(null)

const columnModalOpen = ref(false)
const columnForm = reactive({ name: '', order: 1 })

const loadProjects = async () => {
  const response = await api<BaseResponse<Project[]>>('/api/projects')
  projects.value = response.data || []
  if (!filters.projectId && projects.value.length) filters.projectId = projects.value[0].id
}

const loadUsers = async () => {
  const response = await api<BaseResponse<User[]>>('/api/users')
  users.value = response.data || []
}

const loadColumns = async () => {
  if (!filters.projectId) return
  const response = await api<BaseResponse<TaskColumn[]>>(`/api/columns/${filters.projectId}`)
  columns.value = response.data || []
}

const buildTaskQuery = () => {
  const query = new URLSearchParams()
  if (filters.projectId) query.set('projectId', filters.projectId)
  if (filters.assigneeId) query.set('assigneeId', filters.assigneeId)
  if (filters.priority) query.set('priority', String(filters.priority))
  if (filters.statusId) query.set('statusId', filters.statusId)
  if (filters.dueDateUntil) query.set('dueDateUntil', filters.dueDateUntil)
  return query.toString()
}

const loadTasks = async () => {
  loading.value = true
  const response = await api<BaseResponse<WorkTask[]>>(`/api/tasks?${buildTaskQuery()}`)
  tasks.value = response.data || []
  loading.value = false
}

const loadDetails = async (task: WorkTask) => {
  selectedTask.value = task
  detailsModalOpen.value = true
  const [historyRes, commentRes] = await Promise.all([
    api<BaseResponse<TaskHistory[]>>(`/api/tasks/${task.id}/history`),
    api<BaseResponse<Comment[]>>(`/api/comments/${task.id}`)
  ])
  taskHistory.value = historyRes.data || []
  comments.value = commentRes.data || []
}

const createOrUpdateTask = async (payload: { title: string, description: string, priority: number, dueDate: string | null, projectId: string, assigneeId: string | null, statusId: string, progress: number }) => {
  if (editingTask.value) await api(`/api/tasks/${editingTask.value.id}`, { method: 'PUT', body: payload })
  else await api('/api/tasks', { method: 'POST', body: payload })
  taskModalOpen.value = false
  editingTask.value = null
  await Promise.all([loadColumns(), loadTasks()])
}

const deleteTask = async (id: string) => {
  await api(`/api/tasks/${id}`, { method: 'DELETE' })
  await loadTasks()
}

const createColumn = async () => {
  if (!filters.projectId || !columnForm.name) return
  const order = columns.value.length ? Math.max(...columns.value.map(x => x.order)) + 1 : 1
  await api('/api/columns', { method: 'POST', body: { projectId: filters.projectId, name: columnForm.name, order } })
  columnModalOpen.value = false
  columnForm.name = ''
  columnForm.order = 1
  await loadColumns()
}

const deleteColumn = async (id: string) => {
  await api(`/api/columns/${id}`, { method: 'DELETE' })
  await loadColumns()
}

const moveTask = async (task: WorkTask, statusId: string) => {
  if (task.statusId === statusId) return
  await api(`/api/tasks/${task.id}`, {
    method: 'PUT',
    body: {
      title: task.title,
      description: task.description,
      priority: task.priority,
      dueDate: task.dueDate,
      statusId,
      projectId: task.projectId,
      assigneeId: task.assigneeId,
      progress: task.progress
    }
  })
  await loadTasks()
}

const handleDrop = async (event: DragEvent, columnId: string) => {
  const id = event.dataTransfer?.getData('taskId')
  const task = tasks.value.find(t => t.id === id)
  if (task) await moveTask(task, columnId)
}

const handleDragStart = (event: DragEvent, taskId: string) => {
  event.dataTransfer?.setData('taskId', taskId)
}

const openEditTask = (task: WorkTask) => {
  editingTask.value = task
  taskModalOpen.value = true
}

const addComment = async () => {
  if (!selectedTask.value || !commentText.value) return
  await api('/api/comments', { method: 'POST', body: { taskId: selectedTask.value.id, content: commentText.value } })
  commentText.value = ''
  await loadDetails(selectedTask.value)
}

const removeComment = async (id: string) => {
  await api(`/api/comments/${id}`, { method: 'DELETE' })
  if (selectedTask.value) await loadDetails(selectedTask.value)
}

await loadProjects()
await Promise.all([loadUsers(), loadColumns()])
await loadTasks()

watch(() => ({ ...filters }), async () => {
  if (filters.projectId) await loadColumns()
  await loadTasks()
}, { deep: true })

const tasksByColumn = computed(() => columns.value.map(column => ({
  ...column,
  items: tasks.value.filter(task => task.statusId === column.id)
})))
</script>

<template>
  <div class="space-y-4">
    <div class="flex justify-between items-center gap-2">
      <h2 class="text-2xl font-semibold">
        Workspace do Projeto (Kanban)
      </h2>
      <div class="flex gap-2">
        <UButton
          variant="subtle"
          @click="columnModalOpen = true"
        >
          Criar coluna
        </UButton>
        <UButton @click="editingTask = null; taskModalOpen = true">
          Nova tarefa
        </UButton>
      </div>
    </div>

    <TaskFilters
      v-model:project-id="filters.projectId"
      v-model:assignee-id="filters.assigneeId"
      v-model:priority="filters.priority"
      v-model:status-id="filters.statusId"
      v-model:due-date-until="filters.dueDateUntil"
      :projects="projects"
      :users="users"
      :columns="columns"
    />

    <UProgress
      v-if="loading"
      animation="carousel"
    />

    <div class="grid gap-4 md:grid-cols-3 xl:grid-cols-4">
      <UCard
        v-for="column in tasksByColumn"
        :key="column.id"
        @dragover.prevent
        @drop="handleDrop($event, column.id)"
      >
        <template #header>
          <div class="flex items-center justify-between">
            <h3 class="font-semibold">
              {{ column.name }}
            </h3>
            <UButton
              color="error"
              variant="ghost"
              size="xs"
              aria-label="Excluir coluna"
              @click="deleteColumn(column.id)"
            >
              x
            </UButton>
          </div>
        </template>
        <div class="space-y-2 min-h-24">
          <button
            v-for="task in column.items"
            :key="task.id"
            draggable="true"
            class="w-full text-left p-3 rounded border border-default bg-muted/30"
            @dragstart="handleDragStart($event, task.id)"
          >
            <p class="font-medium">
              {{ task.title }}
            </p>
            <p class="text-xs text-muted">
              Prioridade {{ task.priority }} • {{ task.progress }}%
            </p>
            <div class="mt-2 flex gap-2">
              <UButton
                size="xs"
                variant="subtle"
                @click.stop="openEditTask(task)"
              >
                Editar
              </UButton>
              <UButton
                size="xs"
                variant="outline"
                @click.stop="loadDetails(task)"
              >
                Detalhes
              </UButton>
              <UButton
                size="xs"
                color="error"
                variant="soft"
                @click.stop="deleteTask(task.id)"
              >
                Excluir
              </UButton>
            </div>
          </button>
        </div>
      </UCard>
    </div>

    <TaskFormModal
      :open="taskModalOpen"
      :task="editingTask"
      :projects="projects"
      :users="users"
      :columns="columns"
      :default-project-id="filters.projectId"
      @close="taskModalOpen = false"
      @save="createOrUpdateTask"
    />

    <TaskDetailsModal
      :open="detailsModalOpen"
      :task="selectedTask"
      :history="taskHistory"
      :comments="comments"
      :comment-text="commentText"
      @close="detailsModalOpen = false"
      @update-comment-text="commentText = $event"
      @add-comment="addComment"
      @remove-comment="removeComment"
    />

    <UModal
      :open="columnModalOpen"
      title="Nova coluna"
      @update:open="v => !v && (columnModalOpen = false)"
    >
      <template #body>
        <div class="space-y-3">
          <UInput
            v-model="columnForm.name"
            placeholder="Nome da coluna"
          />
          <UInput
            v-model="columnForm.order"
            type="number"
            min="1"
          />
        </div>
      </template>
      <template #footer>
        <UButton @click="createColumn">
          Salvar
        </UButton>
      </template>
    </UModal>
  </div>
</template>
