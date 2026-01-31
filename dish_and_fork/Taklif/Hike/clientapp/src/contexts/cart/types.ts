import { CartItem, MerchandiseReadModel } from '~/types';

type Cart = Record<string, CartItem>;

type CartContext = [
  Cart,
  {
    change: (item: MerchandiseReadModel, count: number) => void;
    decrease: (item: MerchandiseReadModel, count?: number) => void;
    increase: (item: MerchandiseReadModel, count?: number) => void;
  },
];

export type { Cart, CartContext, CartItem };
