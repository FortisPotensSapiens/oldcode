import Box from '@mui/material/Box';
import dayjs from 'dayjs';

import { OrderDeliveryType, OrderReadModelPageResultModel, OrderState } from '~/types';
import { STATUS_TEXT_COLOR_MAPING, STATUS_TEXT_MAPING } from '~/utils';

interface Row {
  buyerName: string;
  number: number;
  id: string;
  summ: number;
  createDate: string;
  status: string;
  isReadyToDelivery: boolean;
  sellerDeliveryTrackingUrl: string | undefined;
}

const getRows = (data: OrderReadModelPageResultModel | undefined): Row[] | undefined => {
  return data?.items?.map((item) => {
    const isReadyToDelivery =
      item.deliveryType === OrderDeliveryType.Now &&
      item.state === OrderState.Paid &&
      !item.deliveryInfo?.sellerDeliveryTrackingUrl;

    return {
      isReadyToDelivery,
      sellerDeliveryTrackingUrl: item.deliveryInfo?.sellerDeliveryTrackingUrl ?? undefined,
      buyerName: item.buyer.userName ?? '',
      createDate: dayjs(item.created).format('DD/MM/YYYY'),
      id: item.id,
      number: item.number,
      status: item.state,
      summ: (item.items ?? []).reduce((acc, item) => {
        return acc + item.amount * (item.price ?? 0);
      }, 0),
    };
  });
};

const getStyledStatus = (status: string | OrderState): JSX.Element => {
  return (
    <Box color={STATUS_TEXT_COLOR_MAPING[status as OrderState] ?? ''}>
      {STATUS_TEXT_MAPING[status as OrderState] ?? ''}
    </Box>
  );
};

export { getRows, getStyledStatus };
export type { Row };
