import { Box, Pagination as MuiPagination, PaginationItem, styled } from '@mui/material';
import { FC } from 'react';
import { createSearchParams, Link, useLocation, useSearchParams } from 'react-router-dom';

import { useConfig } from '~/contexts';

type PaginationProps = {
  pagesCount?: number;
  pageNumber: number;
};

const StyledContainer = styled(Box)(({ theme }) => ({
  width: '100%',
  [theme.breakpoints.down('sm')]: {
    bottom: '55px',
  },
}));

const StyledPagination = styled(MuiPagination)(({ theme }) => ({
  justifyContent: 'center',
  display: 'flex',
}));

const Pagination: FC<PaginationProps> = ({ pageNumber, pagesCount }) => {
  const { pathname } = useLocation();
  const [search] = useSearchParams();
  const { pages } = useConfig();

  const scrollToTop = () => {
    window.scrollTo({
      top: 0,
      behavior: 'smooth',
    });
  };

  if (!pagesCount || pagesCount < 2) {
    return null;
  }

  return (
    <StyledContainer>
      <StyledPagination
        color="primary"
        count={pagesCount}
        page={pageNumber}
        renderItem={(item) => (
          <PaginationItem
            component={Link}
            to={`${pathname}?${createSearchParams({
              ...Object.fromEntries(search.entries()),
              [pages.pageNumberParamName]: String(item.page),
            })}`}
            {...item}
            onClick={scrollToTop}
          />
        )}
        shape="rounded"
      />
    </StyledContainer>
  );
};

export { Pagination };
