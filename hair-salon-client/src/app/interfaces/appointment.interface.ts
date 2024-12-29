export interface Appointment {
  id: number;
  service: string;
  date: Date;
  time: string;
  stylist: string;
  duration: number;
  price: number;
  location: string;
  status?: 'available' | 'confirmed' | 'cancelled';
}