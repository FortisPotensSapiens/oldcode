import { Typography } from '@mui/material';
import { useTranslation } from 'react-i18next';

import { CategoryReadModel } from '~/types';
import { useCategoriesByKind } from '~/utils/categories';
import { kinds } from '~/utils/kind';

import { StorefrontFiltersGroup } from './StorefrontFiltersGroup/StorefrontFiltersGroup';
import { StorefrontFiltersBlockContainer, StyledPapaer, StyledSkeleton } from './styled';

const StorefrontFilters = ({
  categories,
  onChange,
  value,
}: {
  value: Record<string, string[]>;
  categories: CategoryReadModel[] | null | undefined;
  onChange: (data: Record<string, string[]>) => void;
}) => {
  const { t } = useTranslation();
  const categoriesByKind = useCategoriesByKind(categories ?? []);

  if (!categories) {
    return (
      <StyledPapaer>
        <StyledSkeleton />
      </StyledPapaer>
    );
  }

  return (
    <StyledPapaer>
      {kinds.map((kind) => {
        const categories = categoriesByKind[kind];

        return (
          <StorefrontFiltersBlockContainer key={kind}>
            <Typography variant="h6">{t(`enums.type.${kind}`)}</Typography>

            <StorefrontFiltersGroup onChange={onChange} kind={kind} categories={categories} value={value} />
          </StorefrontFiltersBlockContainer>
        );
      })}
    </StyledPapaer>
  );
};

export default StorefrontFilters;
