import { z } from 'zod'
import type { FastifyInstance } from 'fastify'
import { registerLegacyAccount } from '../domain/auth.js'

const registerSchema = z.object({
  username: z.string().trim().min(3).max(64).regex(/^[A-Za-z0-9_]+$/, 'invalid username'),
  password: z.string().min(6).max(256),
  nickname: z.string().trim().min(1).max(32),
  sex: z.boolean().default(false),
})

export async function accountRoutes(app: FastifyInstance) {
  app.post('/api/account/register', async (request, reply) => {
    const result = registerSchema.safeParse(request.body)
    if (!result.success) {
      return reply.code(400).send({ message: '註冊資料格式錯誤', code: 'INVALID_REGISTER_PAYLOAD' })
    }

    const registered = await registerLegacyAccount(result.data)
    if (registered.ok) {
      return reply.code(201).send({ ok: true })
    }

    if (registered.reason === 'USERNAME_EXISTS') {
      return reply.code(409).send({ message: '帳號已存在', code: registered.reason })
    }

    return reply.code(400).send({ message: '註冊失敗', code: registered.reason })
  })
}
