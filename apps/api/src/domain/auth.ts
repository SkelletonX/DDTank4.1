import { createHash } from 'node:crypto'
import sql from 'mssql'
import { env } from '../config.js'
import { findAccountByUsername, createAccount } from '../infrastructure/sqlite.js'
import { getSqlPool } from '../infrastructure/sqlserver.js'

export interface AuthenticatedAccount {
  userId: number
  username: string
  nickname: string
  email: string | null
  sex: boolean
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

export interface RegistrationInput {
  username: string
  password: string
  nickname: string
  sex: boolean
}

export type RegistrationFailure = 'USERNAME_EXISTS' | 'REGISTER_FAILED'

function passwordMd5(password: string) {
  return createHash('md5').update(password, 'utf8').digest('hex').toUpperCase()
}

export async function authenticateLegacyAccount(
  username: string,
  password: string,
): Promise<AuthenticatedAccount | null> {
  const passwordHash = passwordMd5(password)

  if (env.DATABASE_DRIVER === 'sqlite') {
    const account = findAccountByUsername(username)
    if (!account || account.passwordMd5 !== passwordHash) return null

    return {
      userId: account.userId,
      username: account.username,
      nickname: account.nickname,
      email: account.email,
      sex: account.sex !== 0,
      money: account.money,
      giftToken: account.giftToken,
      gold: account.gold,
      gp: account.gp,
      grade: account.grade,
      attack: account.attack,
      defence: account.defence,
      agility: account.agility,
      luck: account.luck,
      winCount: account.winCount,
      totalCount: account.totalCount,
      escapeCount: account.escapeCount,
      repute: account.repute,
      consortiaId: account.consortiaId,
      consortiaName: account.consortiaName,
      offer: account.offer,
      skin: account.skin,
      style: account.style,
      colors: account.colors,
      fightPower: account.fightPower,
    }
  }

  const pool = await getSqlPool()
  const result = await pool.request()
    .input('ApplicationName', sql.VarChar(64), 'DanDanTang')
    .input('UserName', sql.VarChar(64), username)
    .input('Password', sql.VarChar(64), passwordHash)
    .query<{ UserID: number | null }>(`
      DECLARE @UserID INT;
      EXEC Mem_Users_Accede @ApplicationName, @UserName, @Password, @UserID OUTPUT;
      SELECT @UserID AS UserID;
    `)

  const userId = Number(result.recordset[0]?.UserID)
  if (!Number.isInteger(userId) || userId < 0) return null

  const account = await pool.request()
    .input('UserID', sql.Int, userId)
    .query<{ NickName: string | null; Email: string | null }>(`
      SELECT TOP 1 NickName, Email
      FROM Mem_UserInfo
      WHERE UserID = @UserID
    `)

  return {
    userId,
    username,
    nickname: account.recordset[0]?.NickName ?? username,
    email: account.recordset[0]?.Email ?? null,
    sex: false,
    money: 0,
    giftToken: 0,
    gold: 0,
    gp: 0,
    grade: 1,
    attack: 0,
    defence: 0,
    agility: 0,
    luck: 0,
    winCount: 0,
    totalCount: 0,
    escapeCount: 0,
    repute: 0,
    consortiaId: 0,
    consortiaName: '',
    offer: 0,
    skin: '',
    style: ',,,,,,,,',
    colors: ',,,,,,,,',
    fightPower: 0,
  }
}

export async function registerLegacyAccount(
  input: RegistrationInput,
): Promise<{ ok: true } | { ok: false; reason: RegistrationFailure }> {
  const hash = passwordMd5(input.password)

  if (env.DATABASE_DRIVER === 'sqlite') {
    return createAccount({
      username: input.username,
      passwordMd5: hash,
      nickname: input.nickname,
      sex: input.sex,
    })
  }

  const pool = await getSqlPool()
  try {
    const result = await pool.request()
      .input('UserName', sql.VarChar(64), input.username)
      .input('Password', sql.VarChar(256), input.password)
      .input('NickName', sql.VarChar(64), input.nickname)
      .input('Sex', sql.Bit, input.sex)
      .input('Money', sql.Int, 100)
      .input('GiftToken', sql.Int, 100)
      .input('Gold', sql.Int, 100)
      .query<{ Result: number }>(`
        DECLARE @Result INT;
        EXEC @Result = SP_Account_Register @UserName, @Password, @NickName, @Sex, @Money, @GiftToken, @Gold;
        SELECT @Result AS Result;
      `)

    const code = Number(result.recordset[0]?.Result)
    return code === 0
      ? { ok: true }
      : { ok: false, reason: code === 2 ? 'USERNAME_EXISTS' : 'REGISTER_FAILED' }
  } catch {
    return { ok: false, reason: 'REGISTER_FAILED' }
  }
}
