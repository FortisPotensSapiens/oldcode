import { PartnerType } from '~/types';

const partnerMap: Record<PartnerType, string> = {
  [PartnerType.Company]: 'Компания',
  [PartnerType.IndividualEntrepreneur]: 'ИП',
  [PartnerType.SelfEmployed]: 'Самозанятый',
};

type GetPartnerTypeText = (type: PartnerType) => string;

const getPartnerTypeText: GetPartnerTypeText = (type) => partnerMap[type];

export { getPartnerTypeText };
