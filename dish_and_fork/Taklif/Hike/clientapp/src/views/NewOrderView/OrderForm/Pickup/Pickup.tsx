import { PhoneAndroidTwoTone } from '@mui/icons-material';
import { Grid } from '@mui/material';
import dayjs from 'dayjs';
import { FC } from 'react';

import { PartnerReadModel } from '~/types';
import { addressToString } from '~/utils';
import { getWeekDayLocaled } from '~/utils/week';

const Pickup: FC<{
  seller: PartnerReadModel;
  showPhone?: boolean;
}> = ({ seller, showPhone }) => {
  const start = dayjs(seller.workingTime.start).toDate().toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
  const end = dayjs(seller.workingTime.end).toDate().toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });

  return (
    <Grid container spacing={3}>
      <Grid container item sm={12} spacing={2} xs={12}>
        <Grid color={({ palette }) => palette.text.blue} item xs={12}>
          {addressToString(seller.address)}
        </Grid>

        {showPhone ? (
          <Grid item typography="body2" xs={12} style={{ display: 'flex', alignItems: 'center' }}>
            <PhoneAndroidTwoTone style={{ marginRight: '10px' }} /> <strong>{seller.contactPhone}</strong>
          </Grid>
        ) : undefined}

        <Grid color="text.lightGrey" item typography="body2" xs={12}>
          {seller.workingDays?.map((item) => {
            return (
              <div key={item}>
                {getWeekDayLocaled(item)}: с {start} до {end}
              </div>
            );
          })}
        </Grid>
      </Grid>
    </Grid>
  );
};

export { Pickup };
