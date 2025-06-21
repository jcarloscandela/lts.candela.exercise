// src/services/userService.ts

import type { Paginated } from "../models/paginated"
import type { User } from "../models/user"

const API_BASE = import.meta.env.VITE_API_BASE_URL as string + '/users'

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

export async function updateUser(id: number, user: { name: string; email: string }): Promise<void> {
  const response = await fetch(`${API_BASE}/${id}`, {
    method: 'PATCH',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ name: user.name, email: user.email })
  })
  if (!response.ok) {
    let errorMsg = 'Failed to update user'
    try {
      const data = await response.json()
      errorMsg = data.error || errorMsg
    } catch {
      const text = await response.text()
      if (text) errorMsg = text
    }
    throw new Error(errorMsg)
  }
}
