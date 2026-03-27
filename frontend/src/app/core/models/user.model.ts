export class User {
  id: string;
  email: string;
  role: Array<string>;

  constructor(user: any) {
    this.id = user.id || undefined; 
    this.email = user.email || undefined;
    this.role = user.role || undefined;                    
  }
}