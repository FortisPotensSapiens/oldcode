import { FC, useMemo, useState } from 'react';

import { useCartState } from '~/contexts';
import { useGetLastOrderInfo } from '~/hooks';
import { usePlaceOrderParams } from '~/routing';
import { NewOrderView } from '~/views';

const PlaceOrder: FC = () => {
  const { sellerId = '' } = usePlaceOrderParams();
  const cart = useCartState();
  const [lastOrderInfo] = useState(useGetLastOrderInfo());

  const items = useMemo(() => {
    if (!sellerId) {
      return [];
    }

    return Object.values(cart)
      .filter(({ amount, merchandise }) => amount > 0 && merchandise.seller.id === sellerId)
      .sort((a, b) =>
        a.merchandise.title > b.merchandise.title ? 1 : a.merchandise.title < b.merchandise.title ? -1 : 0,
      );
  }, [cart, sellerId]);

  return <NewOrderView {...lastOrderInfo} clearCart items={items} sellerId={sellerId} />;
};

export default PlaceOrder;
