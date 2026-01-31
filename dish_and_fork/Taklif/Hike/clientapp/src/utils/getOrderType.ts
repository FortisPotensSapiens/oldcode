import { OrderType } from '~/types';

export const getOrderType = (type: OrderType) => {
  switch (type) {
    case OrderType.Standard:
      return {
        textColor: '#FFB800',
        text: 'Стандартный',
      };

    case OrderType.Individual:
      return {
        textColor: 'blue',
        text: 'Индивидуальный',
      };

    default:
      return {
        textColor: '#FFB800',
        text: 'Стандартный',
      };
  }
};
