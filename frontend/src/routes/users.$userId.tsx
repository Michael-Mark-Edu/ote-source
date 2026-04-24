import { createFileRoute } from '@tanstack/react-router'
import ProfilePage from '../components/profile/ProfilePage'

export const Route = createFileRoute('/users/$userId')({
  component: ProfilePage,
})