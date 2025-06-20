// src/services/userService.ts
export interface User {
  id: number
  name: string
  email: string
  translationCredits: number
}

export interface Paginated<T> {
  items: T[]
  totalItems: number
  currentPage: number
  totalPages: number
}

const API_BASE = 'https://localhost:7252/api/users'

export async function fetchUsers (
  page: number,
  pageSize: number,
  sortBy?: string,
  sortOrder?: string,
): Promise<Paginated<User>> {
  const params = new URLSearchParams({
    page: page.toString(),
    pageSize: pageSize.toString(),
  })
  if (sortBy) {
    params.append('sortBy', sortBy)
    params.append('sortOrder', sortOrder || 'asc')
  }
  const response = await fetch(`${API_BASE}?${params}`)
  if (!response.ok) {
    throw new Error('Failed to fetch users')
  }
  return await response.json()
}

export async function deleteUserById (id: number): Promise<void> {
  const response = await fetch(`${API_BASE}/${id}`, { method: 'DELETE' })
  if (!response.ok) {
    throw new Error('Delete failed')
  }
}

export async function createUser(user: { name: string; email: string; translationCredits: number }): Promise<User> {
  const response = await fetch(API_BASE, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(user)
  })
  if (!response.ok) {
    const error = await response.text()
    throw new Error(error || 'Failed to create user')
  }
  return await response.json()
}

export async function updateUserCredits(id: number, credits: number): Promise<void> {
  const response = await fetch(`${API_BASE}/${id}/credits`, {
    method: 'PATCH',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(credits)
  })
  if (!response.ok) {
    const error = await response.text()
    throw new Error(error || 'Failed to update credits')
  }
}
