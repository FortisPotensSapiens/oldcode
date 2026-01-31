import { Grid } from '@mui/material';
import { FC, useState } from 'react';
import { UseQueryResult } from 'react-query';

import { EmptyList, Error, LoadingSpinner, PageLayout, Pagination } from '~/components';
import { useConfig } from '~/contexts';
import { usePageNumber } from '~/hooks';
import { OrderReadModelPageResultModel } from '~/types';

import { Order } from './Order';
import { useSorted } from './useSorted';

type OrdersViewProps = UseQueryResult<OrderReadModelPageResultModel>;

const OrdersView: FC<OrdersViewProps> = ({ data, error, isLoading }) => {
  const [disabled, setDisabled] = useState<{
    disabled: boolean;
    orderId: string;
  }>({
    disabled: false,
    orderId: '',
  });
  const [pageNumber] = usePageNumber();
  const sorted = useSorted(data?.items);
  const { orders } = useConfig();

  const setDisabledState = (disabled: boolean, orderId: string) => {
    setDisabled({
      disabled,
      orderId,
    });
  };

  if (error) {
    return <Error />;
  }

  if (isLoading) {
    return <LoadingSpinner />;
  }

  const noData = !data?.totalCount;
  const pagesCount = data && Math.ceil(data.totalCount / orders.pageSize);

  return (
    <PageLayout title="История заказов">
      {noData ? (
        <EmptyList>Сделайте Ваш первый заказ</EmptyList>
      ) : (
        <>
          <Grid container spacing={{ sm: 3, xs: 2 }}>
            {sorted.map((order) => (
              <Order
                key={order.id}
                disabled={disabled.disabled && disabled.orderId === order.id}
                onPay={setDisabledState}
                {...order}
              />
            ))}
          </Grid>

          <Pagination pageNumber={pageNumber} pagesCount={pagesCount} />
        </>
      )}
    </PageLayout>
  );
};

export { OrdersView };
export type { OrdersViewProps };
