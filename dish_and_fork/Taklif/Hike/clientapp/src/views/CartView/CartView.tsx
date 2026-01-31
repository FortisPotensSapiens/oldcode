import { Box, styled } from '@mui/material';
import { FC } from 'react';

import { EmptyList, PageLayout } from '~/components';
import { useCartCount } from '~/hooks';

import { SellerCart } from './SellerCart';
import useSellerGoods from './useSellerGoods';

const StyledGrid = styled(Box)(({ theme }) => {
  const gap = theme.spacing(3);

  return {
    columnGap: gap,
    display: 'grid',
    gridTemplateColumns: 'repeat(1, 1fr)',
    margin: 0,
    padding: 0,
    position: 'relative',
    rowGap: gap,

    [theme.breakpoints.up('xs')]: {
      gridTemplateColumns: 'repeat(1, 1fr)',
      marginBottom: gap,
    },

    [theme.breakpoints.up('md')]: { gridTemplateColumns: 'repeat(3, 1fr)' },
    [theme.breakpoints.up('lg')]: { gridTemplateColumns: 'repeat(4, 1fr)' },
  };
});

const CartView: FC = () => {
  const count = useCartCount();
  const sellerGoods = useSellerGoods();
  const noData = count === 0;

  return (
    <PageLayout title="Корзина">
      {noData ? (
        <EmptyList>Ваша корзина пуста</EmptyList>
      ) : (
        <StyledGrid>
          {sellerGoods.map((merchandises) => {
            // Берем самые последние сохраненные данные продавца
            const [{ merchandise }] = merchandises.sort((a, b) => Number(a.updated) - Number(b.updated));

            return <SellerCart key={merchandise.seller.id} items={merchandises} {...merchandise.seller} />;
          })}
        </StyledGrid>
      )}
    </PageLayout>
  );
};

export { CartView };
