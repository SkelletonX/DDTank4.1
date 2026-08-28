export interface PlayerSave {
  playerId: number;
  version: number;
  data: Record<string, unknown>;
}

/**
 * Persistence boundary for the future save-based architecture.
 * The domain must not know whether the implementation uses SQL, files or another store.
 */
export interface SaveStore {
  load(playerId: number): Promise<PlayerSave | null>;
  save(save: PlayerSave): Promise<void>;
}
