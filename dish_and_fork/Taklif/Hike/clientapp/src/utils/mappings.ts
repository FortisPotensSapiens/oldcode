import { OrderState } from '~/types';

const STATUS_TEXT_MAPING: Record<OrderState, string> = {
  [OrderState.Paid]: 'Оплачено',
  [OrderState.Delivered]: 'Доставлено',
  [OrderState.Created]: 'Новый',
  [OrderState.Delivering]: 'В доставке',
};

const STATUS_TEXT_COLOR_MAPING: Record<OrderState, string> = {
  [OrderState.Paid]: 'brown',
  [OrderState.Delivered]: 'green',
  [OrderState.Created]: 'blue',
  [OrderState.Delivering]: 'blue',
};

export { STATUS_TEXT_COLOR_MAPING, STATUS_TEXT_MAPING };
