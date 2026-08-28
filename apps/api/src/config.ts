import { z } from 'zod'

const envSchema = z.object({
  NODE_ENV: z.enum(['development', 'test', 'production']).default('development'),
  HOST: z.string().default('127.0.0.1'),
  PORT: z.coerce.number().int().positive().default(3000),
  DATABASE_DRIVER: z.enum(['sqlite', 'mssql']).default('sqlite'),
  SQLITE_PATH: z.string().default('./data/dd.sqlite'),
  DB_SERVER: z.string().optional(),
  DB_PORT: z.coerce.number().int().positive().default(1433),
  DB_NAME: z.string().optional(),
  DB_USER: z.string().optional(),
  DB_PASSWORD: z.string().optional(),
  DB_ENCRYPT: z.coerce.boolean().default(false),
  DB_TRUST_SERVER_CERTIFICATE: z.coerce.boolean().default(true),
  SESSION_SECRET: z.string().min(32).default('development-only-change-me-development-only'),
})

export const env = envSchema.parse(process.env)

export function requireDatabaseConfig() {
  const values = {
    server: env.DB_SERVER,
    database: env.DB_NAME,
    user: env.DB_USER,
    password: env.DB_PASSWORD,
  }

  if (Object.values(values).some((value) => !value)) {
    throw new Error('SQL Server configuration is incomplete')
  }

  return values as {
    server: string
    database: string
    user: string
    password: string
  }
}

export function requireSessionSecret() {
  return env.SESSION_SECRET
}
