import Fastify from 'fastify'
import cors from '@fastify/cors'
import cookie from '@fastify/cookie'
import jwt from '@fastify/jwt'
import { env, requireSessionSecret } from './config.js'
import { authRoutes } from './routes/auth.js'
import { accountRoutes } from './routes/account.js'

const app = Fastify({ logger: true })

await app.register(cors, { origin: true, credentials: true })
await app.register(cookie)
await app.register(jwt, {
  secret: requireSessionSecret(),
  cookie: { cookieName: 'dd_session', signed: false },
})
await app.register(authRoutes)
await app.register(accountRoutes)

app.get('/health', async () => ({
  status: 'ok',
  service: 'dd-api',
  environment: env.NODE_ENV,
}))

await app.listen({ port: env.PORT, host: env.HOST })
