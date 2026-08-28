import { z } from 'zod'
import type { FastifyInstance } from 'fastify'
import { authenticateLegacyAccount } from '../domain/auth.js'

const loginSchema = z.object({
  username: z.string().trim().min(1).max(64),
  password: z.string().min(1).max(256),
})

export async function authRoutes(app: FastifyInstance) {
  app.post('/api/auth/login', async (request, reply) => {
    const result = loginSchema.safeParse(request.body)
    if (!result.success) {
      return reply.code(400).send({ message: '帳號或密碼格式錯誤', code: 'INVALID_LOGIN_PAYLOAD' })
    }

    try {
      const account = await authenticateLegacyAccount(result.data.username, result.data.password)
      if (!account) {
        return reply.code(401).send({ message: '帳號或密碼錯誤', code: 'INVALID_CREDENTIALS' })
      }

      const token = await reply.jwtSign(
        { userId: account.userId, username: account.username },
        { expiresIn: '12h' },
      )

      reply.setCookie('dd_session', token, {
        httpOnly: true,
        sameSite: 'lax',
        secure: process.env.NODE_ENV === 'production',
        path: '/',
        maxAge: 60 * 60 * 12,
      })

      return { ok: true, user: account }
    } catch (error) {
      request.log.error({ err: error }, 'legacy authentication failed')
      return reply.code(503).send({ message: '驗證服務暫時無法使用', code: 'AUTH_SERVICE_UNAVAILABLE' })
    }
  })

  app.get('/api/auth/me', async (request, reply) => {
    try {
      const payload = await request.jwtVerify<{ userId: number; username: string }>()
      return { ok: true, user: payload }
    } catch {
      return reply.code(401).send({ message: '尚未登入', code: 'NOT_AUTHENTICATED' })
    }
  })

  app.post('/api/auth/logout', async (_request, reply) => {
    reply.clearCookie('dd_session', { path: '/' })
    return { ok: true }
  })
}
