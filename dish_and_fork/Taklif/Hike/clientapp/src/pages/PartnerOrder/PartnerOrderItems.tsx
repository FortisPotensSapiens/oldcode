import { Box, Grid, Typography } from '@mui/material';
import { FC, useMemo } from 'react';
import { Link } from 'react-router-dom';

import { useCurrencySymbol } from '~/hooks';
import { generateIndividualOrderOfferPath, generateProductPath } from '~/routing';
import { OrderItemReadModel, OrderItemType } from '~/types';
import { getSummServingSize } from '~/utils/getSumServingSize';

import { SummServingSize } from '../PlaceIndividualOrder/IndividualOrderDetail/SummServingSize';

type PartnerOrderItemsProps = { items?: OrderItemReadModel[] | null };

const PartnerOrderItems: FC<PartnerOrderItemsProps> = ({ items }) => {
  const currency = useCurrencySymbol();

  const sum = useMemo(() => items?.reduce((acc, { amount, price }) => acc + amount * (price ?? 0), 0) ?? 0, [items]);
  const servingSumm = useMemo(() => getSummServingSize(items), [items]);

  if (!items?.length) {
    return null;
  }

  return (
    <>
      <Box borderBottom="rgba(0, 0, 0, 0.38)" borderTop="rgba(0, 0, 0, 0.38)" mb={3} mt={3} paddingX={3} px={0}>
        {items.map(({ amount, id, title, price, type, itemId, orderId, offerId }) => (
          <Grid key={`${id}`} container>
            <Grid item xs={8}>
              <Box>
                {String(type) === String(OrderItemType.Standard) ? (
                  <Link
                    to={generateProductPath({
                      productId: String(itemId),
                    })}
                    style={{
                      color: '#000',
                    }}
                  >
                    {title}
                  </Link>
                ) : (
                  <Link
                    to={generateIndividualOrderOfferPath(orderId, String(offerId))}
                    style={{
                      color: '#000',
                    }}
                  >
                    {title}
                  </Link>
                )}
              </Box>
            </Grid>

            <Grid item textAlign="right" xs={2}>
              <Typography variant="body1">
                <Box color="text.lightGrey">{amount} шт.</Box>
              </Typography>
            </Grid>

            <Grid item textAlign="right" xs={2}>
              <Typography variant="body1">
                {amount * (price ?? 0)} {currency}
              </Typography>
            </Grid>
          </Grid>
        ))}
      </Box>

      <Grid container>
        <Grid item xs={6}>
          <Typography variant="h4">Итого</Typography>
        </Grid>

        <Grid item textAlign="right" xs={6}>
          <Typography variant="h4">
            {sum} {currency}
          </Typography>
        </Grid>
      </Grid>

      <SummServingSize size={servingSumm} />
    </>
  );
};

export { PartnerOrderItems };
export type { PartnerOrderItemsProps };
