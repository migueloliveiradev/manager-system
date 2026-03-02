<script setup lang="ts">
type TaskItem = { id: string, title: string, assignee: string, priority: number, dueDate: string, progress: number, history: string[] }
type Column = { id: string, name: string, tasks: TaskItem[] }

const filters = reactive({ assignee: '', priority: 0, dueDate: '' })
const columns = ref<Column[]>([
  { id: 'todo', name: 'Para fazer', tasks: [{ id: '1', title: 'Criar autenticação', assignee: 'Ana', priority: 4, dueDate: '2026-03-05', progress: 30, history: ['Tarefa criada', 'Prioridade alterada para urgente'] }] },
  { id: 'doing', name: 'Fazendo', tasks: [{ id: '2', title: 'Tela kanban', assignee: 'Leo', priority: 3, dueDate: '2026-03-04', progress: 60, history: ['Movida para Fazendo'] }] },
  { id: 'done', name: 'Feito', tasks: [] }
])
const modalTask = ref<TaskItem | null>(null)
const showNewTask = ref(false)
const newTask = reactive({ title: '', assignee: '', priority: 2, dueDate: '' })

const filtered = (items: TaskItem[]) => items.filter(task =>
  (!filters.assignee || task.assignee.toLowerCase().includes(filters.assignee.toLowerCase()))
  && (!filters.priority || task.priority === filters.priority)
  && (!filters.dueDate || task.dueDate <= filters.dueDate)
).sort((a, b) => b.priority - a.priority)

const onDrop = (event: DragEvent, columnId: string) => {
  const taskId = event.dataTransfer?.getData('task')
  const fromId = event.dataTransfer?.getData('column')
  if (!taskId || !fromId || fromId === columnId) return
  const from = columns.value.find(x => x.id === fromId)
  const to = columns.value.find(x => x.id === columnId)
  const task = from?.tasks.find(x => x.id === taskId)
  if (!from || !to || !task) return
  from.tasks = from.tasks.filter(x => x.id !== taskId)
  task.history.unshift(`Movida para ${to.name}`)
  to.tasks.push(task)
}

const createTask = () => {
  if (!newTask.title || !newTask.assignee) return
  columns.value[0]?.tasks.push({ id: crypto.randomUUID(), title: newTask.title, assignee: newTask.assignee, priority: newTask.priority, dueDate: newTask.dueDate || '2026-03-10', progress: 0, history: ['Tarefa criada pelo modal rápido'] })
  Object.assign(newTask, { title: '', assignee: '', priority: 2, dueDate: '' })
  showNewTask.value = false
}

const addColumn = () => columns.value.push({ id: crypto.randomUUID(), name: `Coluna ${columns.value.length + 1}`, tasks: [] })
</script>

<template>
  <div class="space-y-4">
    <div class="flex justify-between items-center">
      <h2 class="text-2xl font-semibold">
        Workspace do Projeto (Kanban)
      </h2>
      <div class="flex gap-2">
        <UButton
          icon="i-lucide-columns-3"
          variant="subtle"
          @click="addColumn"
        >
          Criar nova coluna
        </UButton>
        <UButton
          icon="i-lucide-plus"
          @click="showNewTask = true"
        >
          Nova Tarefa
        </UButton>
      </div>
    </div>

    <UCard>
      <div class="grid md:grid-cols-3 gap-3">
        <UInput
          v-model="filters.assignee"
          placeholder="Filtrar por responsável"
        />
        <USelect
          v-model="filters.priority"
          :items="[{ label: 'Todas prioridades', value: 0 }, { label: 'Média', value: 2 }, { label: 'Alta', value: 3 }, { label: 'Urgente', value: 4 }]"
        />
        <UInput
          v-model="filters.dueDate"
          type="date"
        />
      </div>
    </UCard>

    <div class="grid gap-4 md:grid-cols-3 xl:grid-cols-4">
      <UCard
        v-for="column in columns"
        :key="column.id"
        @dragover.prevent
        @drop="onDrop($event, column.id)"
      >
        <template #header>
          <h3 class="font-semibold">
            {{ column.name }}
          </h3>
        </template>
        <div class="space-y-2 min-h-28">
          <button
            v-for="task in filtered(column.tasks)"
            :key="task.id"
            class="w-full text-left p-3 rounded border border-default bg-muted/30"
            draggable="true"
            @dragstart="$event.dataTransfer?.setData('task', task.id); $event.dataTransfer?.setData('column', column.id)"
            @click="modalTask = task"
          >
            <p class="font-medium">
              {{ task.title }}
            </p>
            <p class="text-xs text-muted">
              {{ task.assignee }} • Prioridade {{ task.priority }}
            </p>
          </button>
        </div>
      </UCard>
    </div>

    <UModal
      v-model:open="showNewTask"
      title="Nova Tarefa"
    >
      <template #body>
        <div class="space-y-3">
          <UInput
            v-model="newTask.title"
            placeholder="Título"
          />
          <UInput
            v-model="newTask.assignee"
            placeholder="Responsável"
          />
          <USelect
            v-model="newTask.priority"
            :items="[{ label: 'Média', value: 2 }, { label: 'Alta', value: 3 }, { label: 'Urgente', value: 4 }]"
          />
          <UInput
            v-model="newTask.dueDate"
            type="date"
          />
        </div>
      </template>
      <template #footer>
        <UButton @click="createTask">
          Salvar
        </UButton>
      </template>
    </UModal>

    <UModal
      :open="!!modalTask"
      title="Detalhes da Tarefa"
      @update:open="v => !v && (modalTask = null)"
    >
      <template #body>
        <div
          v-if="modalTask"
          class="space-y-3"
        >
          <p><strong>{{ modalTask.title }}</strong></p>
          <p>Progresso: {{ modalTask.progress }}%</p>
          <USeparator label="Histórico" />
          <ul class="list-disc pl-5 text-sm space-y-1">
            <li
              v-for="line in modalTask.history"
              :key="line"
            >
              {{ line }}
            </li>
          </ul>
        </div>
      </template>
    </UModal>
  </div>
</template>
