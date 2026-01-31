import { Grid } from '@mui/material';
import { FC } from 'react';

import { LoadingSpinner } from '~/components';
import { OfferSellerReadModel } from '~/types';

import PartnerOffersListItem from './PartnerOffersListItem/PartnerOffersListItem';

type PartnerOffersListProps = {
  rows?: OfferSellerReadModel[];
  isLoading?: boolean;
  onDelete: (offerId: string) => void;
};

const PartnerOffersList: FC<PartnerOffersListProps> = ({ children, isLoading, rows, onDelete }) => {
  return (
    <Grid container>
      {isLoading ? <LoadingSpinner /> : undefined}

      {rows?.map((row) => (
        <PartnerOffersListItem key={row.id} row={row} onDelete={onDelete} />
      ))}

      {children && (
        <Grid item xs={12}>
          {children}
        </Grid>
      )}
    </Grid>
  );
};
export { PartnerOffersList };
