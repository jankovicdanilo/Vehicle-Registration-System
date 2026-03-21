export class Insurance {
  id: string;
  name: string;

  constructor(insurance?: any) {
    this.id = insurance?.id || undefined;
    this.name = insurance?.name || undefined;
  }
}