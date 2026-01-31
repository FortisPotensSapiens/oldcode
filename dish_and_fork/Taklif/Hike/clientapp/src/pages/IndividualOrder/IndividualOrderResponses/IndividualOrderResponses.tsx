import { Box, Grid, List, Typography } from '@mui/material';
import { useNavigate } from 'react-router-dom';

import { generatePlaceIndividualOrderPath } from '~/routing';
import { generateIndividualOrderOfferPath } from '~/routing/individualOrder';
import { ApplicationDetailsReadModel, OfferReadModel, PartnerReadModel, UserProfileDetailsReadModel } from '~/types';

import { IndividualOrderResponsesItem } from './IndividualOrderResponsesItem';

const IndividualOrderResponses = ({
  offers,
  onChatOpen,
  individualOrderId,
  showSelectSeller = false,
  my,
  partnerMy,
  individualOrder,
}: {
  offers: OfferReadModel[];
  onChatOpen: (sellerId: string) => void;
  individualOrderId: string;
  showSelectSeller: boolean;
  my: UserProfileDetailsReadModel;
  partnerMy: PartnerReadModel | undefined;
  individualOrder: ApplicationDetailsReadModel | undefined;
}) => {
  const navigate = useNavigate();

  const onClick = (offerId: string) => {
    navigate(generateIndividualOrderOfferPath(individualOrderId, offerId));
  };

  const onOrder = (offerId: string) => {
    navigate(generatePlaceIndividualOrderPath(individualOrderId, offerId));
  };

  if (!offers.length) {
    return (
      <Grid alignContent="center" container height="100%" justifyContent="center">
        <Grid item>
          <Typography>Список откликов пока пуст</Typography>
        </Grid>
      </Grid>
    );
  }

  return (
    <List>
      {offers?.map((row) => {
        return (
          <Box key={row.id} marginBottom={2}>
            <IndividualOrderResponsesItem
              showChat={my?.id === individualOrder?.customer?.id || partnerMy?.id === row.seller.id}
              showSelectSeller={showSelectSeller && !individualOrder?.selectedOrderId}
              onOrder={onOrder}
              onClick={onClick}
              onChatOpen={onChatOpen}
              response={row}
            />
          </Box>
        );
      })}
    </List>
  );
};

export { IndividualOrderResponses };
