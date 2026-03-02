export type BaseResponse<T> = {
  data: T
  errors: string[]
  hasErrors: boolean
}

export type AuthResponse = {
  accessToken: string
  refreshToken: string
  expiresAtUtc: string
}

export type User = {
  id: string
  fullName: string
  email: string
  profilePhotoUrl?: string
  isActive: boolean
}

export type Project = {
  id: string
  name: string
  description: string
  status: number
  createdById: string
}

export type TaskColumn = {
  id: string
  projectId: string
  name: string
  order: number
}

export type WorkTask = {
  id: string
  title: string
  description: string
  priority: number
  statusId: string
  dueDate?: string
  projectId: string
  assigneeId?: string
  progress: number
}

export type TaskHistory = {
  id: string
  taskId: string
  userId: string
  action: string
  createdAtUtc: string
}

export type Comment = {
  id: string
  taskId: string
  userId: string
  content: string
  createdAtUtc: string
}
