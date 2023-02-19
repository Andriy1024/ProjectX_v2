export class RealtimeMessageTypes {
  public static readonly NoteUpdated = "NoteUpdated";
  public static readonly NoteCreated = "NoteCreated";
  public static readonly NoteDeleted = "NoteDeleted";

  public static readonly TaskCreated = "TaskCreated";
  public static readonly TaskUpdated = "TaskUpdated";
  public static readonly TaskDeleted = "TaskDeleted";

  public static readonly BookmarkCreated = "BookmarkCreated";
  public static readonly BookmarkUpdated = "BookmarkUpdated";
  public static readonly BookmarkDeleted = "BookmarkDeleted";

  public static isNoteMessage(type: string): boolean {
    return type === RealtimeMessageTypes.NoteCreated
        || type === RealtimeMessageTypes.NoteUpdated
        || type === RealtimeMessageTypes.NoteDeleted
  }

  public static isTaskMessage(type: string): boolean {
    return type === RealtimeMessageTypes.TaskCreated
        || type === RealtimeMessageTypes.TaskUpdated
        || type === RealtimeMessageTypes.TaskDeleted
  }

  public static isBookmarkMessage(type: string): boolean {
    return type === RealtimeMessageTypes.BookmarkCreated
        || type === RealtimeMessageTypes.BookmarkUpdated
        || type === RealtimeMessageTypes.BookmarkDeleted
  }
}
