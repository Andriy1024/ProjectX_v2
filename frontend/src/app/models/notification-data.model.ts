export class NotificationData {
  public text: string;
  public duration: number;

  constructor(text: string, duration: number = 1000) {
    this.text = text;
    this.duration = duration;
  }
}
