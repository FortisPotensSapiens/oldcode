import { Box, Button, Grid, Paper } from '@mui/material';
import { format } from 'date-fns';
import dayjs from 'dayjs';
import utc from 'dayjs/plugin/utc';
import { FC } from 'react';
import { Link } from 'react-router-dom';

import { useCurrencySymbol, useOnOff } from '~/hooks';
import { generatePartnerNewAppPath } from '~/routing';
import { ApplicationReadModel } from '~/types';

import { EllipsisText } from './EllipsisText';
import { FormDialog } from './FormDialog';
import { getDatesText } from './getDatesText';
import { getSumText } from './getSumText';

dayjs.extend(utc);

type NewAppItemProps = ApplicationReadModel & { onComplete?: (id: string, id2: string) => void };

const NewAppItem: FC<NewAppItemProps> = (props) => {
  const {
    created,
    description,
    fromDate,
    id: appId,
    onComplete,
    sumFrom,
    sumTo,
    title,
    toDate,
    updated,
    selectedOfferId,
  } = props;
  const currency = useCurrencySymbol();
  const [visible, { off: hide, on: show }] = useOnOff();

  const onCompleteCallback = (offerId: string) => {
    onComplete && onComplete(appId, offerId);
  };

  return (
    <>
      <Box borderRadius={({ shape }) => shape.bigBorderRadius} component={Paper} p={2}>
        <EllipsisText color="text.lightGrey" fontSize="0.75em" fontStyle="italic" mt={0} variant="body2">
          {dayjs(updated ?? created)
            .utc()
            .format('LLL')}
        </EllipsisText>

        <EllipsisText variant="h5" whiteSpace="nowrap">
          {title}
        </EllipsisText>

        <EllipsisText color="text.lightGrey" variant="body2">
          {description ?? <br />}
        </EllipsisText>

        <EllipsisText>Когда: {getDatesText(fromDate, toDate) ?? 'не указано'}</EllipsisText>
        <EllipsisText mb={1}>Сумма: {getSumText(currency, sumFrom, sumTo) ?? 'не указано'}</EllipsisText>

        <Grid container spacing={2}>
          <Grid item xs={6}>
            <Button
              color="primary"
              component={Link}
              fullWidth
              to={generatePartnerNewAppPath(appId)}
              variant="contained"
            >
              Подробнее
            </Button>
          </Grid>

          {!selectedOfferId ? (
            <Grid item xs={6}>
              <Button color="primary" fullWidth onClick={show} variant="outlined">
                Откликнуться
              </Button>
            </Grid>
          ) : undefined}
        </Grid>
      </Box>

      <FormDialog applicationId={appId} onClose={hide} onComplete={onCompleteCallback} open={visible} />
    </>
  );
};

export { NewAppItem };
export type { NewAppItemProps };
