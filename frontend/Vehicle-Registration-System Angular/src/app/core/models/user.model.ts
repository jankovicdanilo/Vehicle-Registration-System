export class User {
  id: string;
  name: string;
  email: string;
  role: Array<string>;

  constructor(user) {
    this.id = user.id || undefined;
    this.name = user.name || undefined; 
    this.email = user.email || undefined;
    this.role = user.role || undefined;                    
  }
}