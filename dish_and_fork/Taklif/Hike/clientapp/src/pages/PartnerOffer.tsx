import { FC } from 'react';

import { PageLayout } from '~/components';
import { getPartnerOffersPath } from '~/routing';

const PartnerOffer: FC = () => <PageLayout href={getPartnerOffersPath()} title="Отклик на заявку" />;

export default PartnerOffer;
