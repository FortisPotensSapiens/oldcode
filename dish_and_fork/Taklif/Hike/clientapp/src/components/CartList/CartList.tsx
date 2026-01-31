import { FC } from 'react';

import { CartItem } from '~/types';

import CartListItem from './CartListItem/CartListItem';
import StyledContainer, { StyledContainerProps } from './StyledContainer';

type CartListProps = StyledContainerProps & { items: CartItem[] };

const CartList: FC<CartListProps> = ({ items, scrolled }) => {
  return (
    <StyledContainer scrolled={scrolled}>
      {items.map((item) => {
        return <CartListItem key={item.merchandise.id} item={item} />;
      })}
    </StyledContainer>
  );
};

export { CartList };
export type { CartListProps };
