import { AddressReadModel, OrderCreateModel } from '~/types';

import { OrderVariant } from './OrderForm';

export type FormData = Pick<AddressReadModel, 'apartmentNumber' | 'entrance' | 'house' | 'intercom' | 'street'> &
  Pick<OrderCreateModel, 'comments' | 'recipientFullName' | 'recipientPhone'> & {
    variant: OrderVariant;
  };
