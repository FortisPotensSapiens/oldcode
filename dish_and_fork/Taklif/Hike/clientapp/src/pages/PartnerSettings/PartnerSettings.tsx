import { Box, Typography } from '@mui/material';
import { FC } from 'react';

import { Breadcrumbs, Link } from '~/components';
import { getStorefrontPath } from '~/routing';
import { PartnerSettingsView } from '~/views';

const PartnerSettings: FC = () => (
  <Box alignItems="center" display="flex" flexDirection="column">
    <Breadcrumbs>
      <Link key="root" color="text.secondary" sx={{ textDecoration: 'none' }} to={getStorefrontPath()}>
        Все изделия
      </Link>

      <Typography key="title" color="text.primary">
        Настройки магазина
      </Typography>
    </Breadcrumbs>

    <Typography component="h1" textAlign="center" variant="h3">
      Настройки магазина
    </Typography>

    <PartnerSettingsView />
  </Box>
);

export { PartnerSettings };
