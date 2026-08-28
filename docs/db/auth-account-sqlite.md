# Auth / Account SQLite schema

這份 schema 是從舊 `Request/Login.ashx.cs`、`AccountRegister.ashx.cs`、`PlayerBussiness` 的登入/註冊欄位反推而來，目標是先支撐 Vue + Node 新流程，不追求一次還原完整 DDTank DB。

## `users`

| SQLite | Legacy meaning | Purpose |
|---|---|---|
| `UserID` | `PlayerInfo.ID` / `UserID` | 玩家/帳號主鍵 |
| `UserName` | `UserName` | 登入帳號 |
| `Password` | legacy password contract | 儲存 UPPER(MD5(password))，與既有登入相容 |
| `NickName` | `PlayerInfo.NickName` | 角色名稱 |
| `Sex` | `PlayerInfo.Sex` | 性別 |
| `Email` | `Mem_UserInfo.Email` | 帳號信箱，現階段可為 null |
| `Money` | `PlayerInfo.Money` | 軟貨幣 |
| `GiftToken` | `PlayerInfo.GiftToken` | 禮券/代幣 |
| `Gold` | `PlayerInfo.Gold` | 金幣 |
| `GP` | `PlayerInfo.GP` | 經驗值 |
| `Grade` | `PlayerInfo.Grade` | 等級 |
| `Attack` | `PlayerInfo.Attack` | 攻擊 |
| `Defence` | `PlayerInfo.Defence` | 防禦 |
| `Agility` | `PlayerInfo.Agility` | 敏捷 |
| `Luck` | `PlayerInfo.Luck` | 幸運 |
| `WinCount` | `PlayerInfo.Win` | 勝場 |
| `TotalCount` | `PlayerInfo.Total` | 總場次 |
| `EscapeCount` | `PlayerInfo.Escape` | 逃跑次數 |
| `Repute` | `PlayerInfo.Repute` | 聲望 |
| `ConsortiaID` | `PlayerInfo.ConsortiaID` | 公會 ID |
| `ConsortiaName` | `PlayerInfo.ConsortiaName` | 公會名稱快取 |
| `Offer` | `PlayerInfo.Offer` | 公會貢獻相關數值 |
| `Skin` | `PlayerInfo.Skin` | 角色外觀 |
| `Style` | `PlayerInfo.Style` | 裝扮欄位 |
| `Colors` | `PlayerInfo.Colors` | 顏色欄位 |
| `FightPower` | `PlayerInfo.FightPower` | 戰鬥力 |
| `CreatedAt` | new | 建立時間 |

## 為什麼先合併 account / player？

舊系統把會員帳號與遊戲角色資料分散在 Membership / Tank DB，但目前要先讓登入、註冊能跑。把最小必要欄位放在同一張 `users` 表，可以先完成：

`register → login → session → /me → 後續玩家資料`

等玩家、背包、公會等功能開始重構，再依領域拆成 `accounts`、`players`、`items`、`consortia` 等表即可。
