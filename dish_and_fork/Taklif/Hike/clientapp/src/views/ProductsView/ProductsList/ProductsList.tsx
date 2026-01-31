import { Box } from '@mui/system';
import { FC } from 'react';

import { Grid } from '~/components';
import { MerchandiseReadModel } from '~/types';

import ProductsListItem, { ProductsListItemProps } from './ProductsListItem';

type ProductsListProps = Pick<ProductsListItemProps, 'hideSeller' | 'addToCard' | 'customActions' | 'showTags'> & {
  items: MerchandiseReadModel[];
};

const ProductsList: FC<ProductsListProps> = ({ hideSeller, items, addToCard, customActions, showTags }) => (
  <Box paddingTop={2} paddingBottom={2}>
    <Grid
      sx={{
        listStyleType: 'none',
        rowGap: {
          sm: 5,
        },
      }}
    >
      {items.map((item) => (
        <ProductsListItem
          key={item.id}
          customActions={customActions}
          addToCard={addToCard}
          hideSeller={hideSeller}
          merchandise={item}
          showTags={showTags}
        />
      ))}
    </Grid>
  </Box>
);

export type { ProductsListProps };
export { ProductsList };
