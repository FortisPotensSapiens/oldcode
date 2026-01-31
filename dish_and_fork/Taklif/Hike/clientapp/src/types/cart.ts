import { MerchandiseReadModel } from './swagger';

export type CartItem = {
  amount: number;
  merchandise: MerchandiseReadModel;
  updated: Date;
};
