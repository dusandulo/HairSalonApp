export interface Appointment {
    id: number;
    date: Date;
    time: string;
    service: string;
    status: string;
    price: number;
    notes?: string;
    clientId?: string;
    stylistId: string;
    createdAt: Date;
    updatedAt: Date;
  }