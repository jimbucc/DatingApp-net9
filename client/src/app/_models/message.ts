export interface Message {
  id: number;
  senderId: number;
  sendUserName: string;
  senderPhotoUrl: string;
  recipientId: number;
  recipientPhotoUrl: string;
  recipientUserName: string;
  content: string;
  dateRead?: Date;
  messageSent: Date;
}
