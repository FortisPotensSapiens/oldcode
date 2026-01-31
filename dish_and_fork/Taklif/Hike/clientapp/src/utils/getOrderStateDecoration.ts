import { AlertProps } from '@mui/material';

import { OrderState } from '~/types';

type GetOrderStateDecoration = (state: OrderState) => Pick<AlertProps, 'severity'> & {
  text: string;
};

const getOrderStateDecoration: GetOrderStateDecoration = (state) => {
  switch (state) {
    case OrderState.Paid:
      return {
        severity: 'success',
        text: 'Оплачен',
      };

    case OrderState.Delivered:
      return {
        severity: 'success',
        text: 'Завершен',
      };

    case OrderState.Delivering:
      return {
        severity: 'success',
        text: 'В доставке',
      };

    default:
      return {
        severity: 'warning',
        text: 'Не оплачен',
      };
  }
};

export { getOrderStateDecoration };
export type { GetOrderStateDecoration };
