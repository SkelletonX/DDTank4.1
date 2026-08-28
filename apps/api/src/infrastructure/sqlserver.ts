import sql from 'mssql'
import { env, requireDatabaseConfig } from '../config.js'

let poolPromise: Promise<sql.ConnectionPool> | undefined

export function getSqlPool() {
  if (!poolPromise) {
    const db = requireDatabaseConfig()
    poolPromise = new sql.ConnectionPool({
      server: db.server,
      database: db.database,
      user: db.user,
      password: db.password,
      port: env.DB_PORT,
      options: {
        encrypt: env.DB_ENCRYPT,
        trustServerCertificate: env.DB_TRUST_SERVER_CERTIFICATE,
      },
      pool: {
        min: 0,
        max: 10,
        idleTimeoutMillis: 30_000,
      },
    }).connect()
  }

  return poolPromise
}

export async function closeSqlPool() {
  if (!poolPromise) return

  const pool = await poolPromise
  await pool.close()
  poolPromise = undefined
}
