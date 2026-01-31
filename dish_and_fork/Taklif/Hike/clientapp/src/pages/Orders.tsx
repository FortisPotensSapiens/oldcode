import { FC } from 'react';

import { useGetOrdersByFilter } from '~/api';
import { useConfig } from '~/contexts';
import { usePageNumber } from '~/hooks';
import { OrdersView } from '~/views';

const Orders: FC = () => {
  const [pageNumber] = usePageNumber();
  const config = useConfig();
  const { pageSize } = config.orders;
  const orders = useGetOrdersByFilter({ pageNumber, pageSize });

  return <OrdersView {...orders} />;
};

export default Orders;
