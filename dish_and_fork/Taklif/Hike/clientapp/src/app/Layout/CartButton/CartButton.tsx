import { FC } from 'react';

import { ReactComponent as CartIcon } from '~/assets/icons/cart-small.svg';
import { MenuButton } from '~/components';
import { useCartCount } from '~/hooks';
import { getCartPath } from '~/routing';

import { StyledBadge } from './StyledBadge';

const CartButton: FC = () => {
  const count = useCartCount();

  return (
    <MenuButton
      icon={
        <StyledBadge badgeContent={count || undefined} color="primary">
          <CartIcon />
        </StyledBadge>
      }
      to={getCartPath()}
    >
      Корзина
    </MenuButton>
  );
};

export { CartButton };
