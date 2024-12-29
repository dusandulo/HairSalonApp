export interface Profile {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  preferences?: {
    appointmentReminders: boolean;
    marketingEmails: boolean;
  };
}
