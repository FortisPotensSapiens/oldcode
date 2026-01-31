import { Box, styled, Typography } from '@mui/material';
import { useNavigate } from 'react-router-dom';

import { EllipsisBox } from '~/components';
import { useCurrencySymbol } from '~/hooks';
import { generateProductPath } from '~/routing';
import { CartItem } from '~/types';

import { CartItemChange } from '../../CartItemChange';

const StyledItemContainer = styled('li', { name: 'StyledItemContainer' })(({ theme }) => ({
  '&:first-of-type': {
    marginTop: 0,
  },

  display: 'grid',
  gridTemplateAreas: '"image title title" "image count sum"',
  gridTemplateColumns: 'auto auto 1fr',
  gridTemplateRows: '1fr auto',
  marginBottom: 0,
  marginTop: theme.spacing(1.25),
}));

const StyledImageContainer = styled('div', { name: 'StyledImageContainer' })(({ theme }) => ({
  backgroundColor: theme.palette.grey['400'],
  backgroundPosition: 'center center',
  backgroundSize: 'cover',
  borderRadius: Number(theme.shape.borderRadius) * 2.5,
  gridArea: 'image',
  height: 70,
  marginRight: theme.spacing(2.5),
  width: 82,
  cursor: 'pointer',
}));

const CartListItem = ({ item }: { item: CartItem }) => {
  const currency = useCurrencySymbol();
  const { merchandise } = item;
  const [image] = merchandise.images ?? [];
  const navigate = useNavigate();

  const onProductClick = () => {
    navigate(generateProductPath({ productId: merchandise.id }));
  };

  return (
    <StyledItemContainer>
      <StyledImageContainer onClick={onProductClick} style={{ backgroundImage: `url(${image?.path})` }} />

      <EllipsisBox gridArea="title">{merchandise.title}</EllipsisBox>
      <Box component={CartItemChange} gridArea="count" merchandise={merchandise} />

      <Typography alignSelf="center" display="flex" gridArea="sum" justifySelf="right" ml={0.5} whiteSpace="nowrap">
        {(merchandise.price * item.amount).toLocaleString()} {currency}
      </Typography>
    </StyledItemContainer>
  );
};

export default CartListItem;
