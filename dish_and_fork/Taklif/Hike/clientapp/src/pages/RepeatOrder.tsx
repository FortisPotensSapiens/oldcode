import { FC, useMemo } from 'react';

import { useGetOrderById } from '~/api';
import { Error, LoadingSpinner } from '~/components';
import { useRepeatOrderParams } from '~/routing';
import {
  CartItem,
  MerchandiseReadModel,
  MerchandiseUnitType,
  OrderItemReadModel,
  OrderReadModel,
  PartnerReadModel,
  PartnerState,
  PartnerType,
  SellerShortInfoReadModel,
} from '~/types';
import { NewOrderView } from '~/views';

function toMerch(data: OrderReadModel, item: OrderItemReadModel): MerchandiseReadModel {
  return {
    seller: toShort(data.seller),
    unitType: MerchandiseUnitType.Pieces,
    title: item.title ?? '',
    id: item.itemId || item.offerId || item.id,
    price: item.price ?? 0,
    currencyType: item.currencyType,
    state: item.state,
    servingSize: item.servingSize ?? 0,
    created: item.created,
    servingGrossWeightInKilograms: item.servingGrossWeightInKilograms,
    availableQuantity: 0,
    isTagsAppovedByAdmin: true,
  };
}

function toShort(seller: SellerShortInfoReadModel): PartnerReadModel {
  return {
    ...seller,
    image: { id: '', title: '', size: 0, created: '' },
    state: PartnerState.Confirmed,
    type: PartnerType.SelfEmployed,
    inn: '',
    contactEmail: '',
    workingTime: { start: '', end: '' },
    address: { zipCode: '0', street: '', house: '', longitude: 0, latitude: 0 },
    isPickupEnabled: true,
    registrationAddress: {
      street: '',
      house: '',
      latitude: 0,
      longitude: 0,
    },
  };
}

const RepeatOrder: FC = () => {
  const { orderId = '' } = useRepeatOrderParams();
  const { data, isError, isLoading } = useGetOrderById(orderId);

  const items = useMemo<CartItem[]>(() => {
    const items = data?.items;

    if (!items) {
      return [];
    }

    const updated = new Date();

    return items.map<CartItem>((item) => ({ amount: item.amount, merchandise: toMerch(data, item), updated }));
  }, [data]);

  if (isError) {
    return <Error />;
  }

  if (isLoading || !data) {
    return <LoadingSpinner />;
  }

  const { recipientAddress, ...recipient } = data;

  return (
    <NewOrderView
      {...recipientAddress}
      {...recipient}
      items={items}
      recipientPhone={data.recipientPhone ?? ''}
      sellerId={data.seller.id}
    />
  );
};

export default RepeatOrder;
