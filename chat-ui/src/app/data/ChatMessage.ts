export interface chatMesage {
  Text: string;
  ConnectionId: string;
  DateTime: Date;
  Time: string;
  SenderId: any;
  ReceiverId:any;
  ChatId: number;
  IsDeleteFromReceiver:boolean;
  IsDeleteFromSender:boolean;
  IsChnaged:boolean;
}
