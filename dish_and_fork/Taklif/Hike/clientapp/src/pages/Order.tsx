import { FC } from 'react';

import { useGetOrderById } from '~/api';
import { useOrderParams } from '~/routing';
import { OrderView } from '~/views';

const Order: FC = () => {
  const { orderId = '' } = useOrderParams();
  const order = useGetOrderById(orderId);

  return <OrderView {...order} />;
};

export default Order;
