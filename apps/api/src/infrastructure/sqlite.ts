import fs from 'node:fs'
import path from 'node:path'
import Database from 'better-sqlite3'
import { env } from '../config.js'

export interface SqliteAccount {
  userId: number
  username: string
  passwordMd5: string
  nickname: string
  sex: number
  email: string | null
  money: number
  giftToken: number
  gold: number
  gp: number
  grade: number
  attack: number
  defence: number
  agility: number
  luck: number
  winCount: number
  totalCount: number
  escapeCount: number
  repute: number
  consortiaId: number
  consortiaName: string
  offer: number
  skin: string
  style: string
  colors: string
  fightPower: number
}

let db: Database.Database | undefined

export function getSqliteDb(): Database.Database {
  if (db) return db

  const filename = path.resolve(env.SQLITE_PATH)
  fs.mkdirSync(path.dirname(filename), { recursive: true })
  db = new Database(filename)
  db.pragma('journal_mode = WAL')
  db.pragma('foreign_keys = ON')
  db.exec(`
    CREATE TABLE IF NOT EXISTS users (
      UserID INTEGER PRIMARY KEY AUTOINCREMENT,
      UserName TEXT NOT NULL UNIQUE,
      Password TEXT NOT NULL,
      NickName TEXT NOT NULL,
      Sex INTEGER NOT NULL DEFAULT 0,
      Email TEXT,
      Money INTEGER NOT NULL DEFAULT 100,
      GiftToken INTEGER NOT NULL DEFAULT 100,
      Gold INTEGER NOT NULL DEFAULT 100,
      GP INTEGER NOT NULL DEFAULT 1,
      Grade INTEGER NOT NULL DEFAULT 1,
      Attack INTEGER NOT NULL DEFAULT 0,
      Defence INTEGER NOT NULL DEFAULT 0,
      Agility INTEGER NOT NULL DEFAULT 0,
      Luck INTEGER NOT NULL DEFAULT 0,
      WinCount INTEGER NOT NULL DEFAULT 0,
      TotalCount INTEGER NOT NULL DEFAULT 0,
      EscapeCount INTEGER NOT NULL DEFAULT 0,
      Repute INTEGER NOT NULL DEFAULT 0,
      ConsortiaID INTEGER NOT NULL DEFAULT 0,
      ConsortiaName TEXT NOT NULL DEFAULT '',
      Offer INTEGER NOT NULL DEFAULT 0,
      Skin TEXT NOT NULL DEFAULT '',
      Style TEXT NOT NULL DEFAULT ',,,,,,,,',
      Colors TEXT NOT NULL DEFAULT ',,,,,,,,',
      FightPower INTEGER NOT NULL DEFAULT 0,
      CreatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP
    );
  `)

  return db
}

export function findAccountByUsername(username: string): SqliteAccount | null {
  const row = getSqliteDb().prepare(`
    SELECT
      UserID as userId,
      UserName as username,
      Password as passwordMd5,
      NickName as nickname,
      Sex as sex,
      Email as email,
      Money as money,
      GiftToken as giftToken,
      Gold as gold,
      GP as gp,
      Grade as grade,
      Attack as attack,
      Defence as defence,
      Agility as agility,
      Luck as luck,
      WinCount as winCount,
      TotalCount as totalCount,
      EscapeCount as escapeCount,
      Repute as repute,
      ConsortiaID as consortiaId,
      ConsortiaName as consortiaName,
      Offer as offer,
      Skin as skin,
      Style as style,
      Colors as colors,
      FightPower as fightPower
    FROM users
    WHERE UserName = ?
  `).get(username) as SqliteAccount | undefined

  return row ?? null
}

export function createAccount(input: {
  username: string
  passwordMd5: string
  nickname: string
  sex: boolean
}): { ok: true } | { ok: false; reason: 'USERNAME_EXISTS' | 'REGISTER_FAILED' } {
  try {
    const result = getSqliteDb().prepare(`
      INSERT INTO users (UserName, Password, NickName, Sex)
      VALUES (?, ?, ?, ?)
    `).run(input.username, input.passwordMd5, input.nickname, input.sex ? 1 : 0)

    return result.changes === 1
      ? { ok: true }
      : { ok: false, reason: 'REGISTER_FAILED' }
  } catch (error: unknown) {
    const message = error instanceof Error ? error.message : ''
    if (message.includes('UNIQUE constraint failed: users.UserName')) {
      return { ok: false, reason: 'USERNAME_EXISTS' }
    }
    return { ok: false, reason: 'REGISTER_FAILED' }
  }
}

export function closeSqliteDb() {
  db?.close()
  db = undefined
}
