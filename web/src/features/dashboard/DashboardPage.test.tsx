import { describe, expect, it } from 'vitest';
import { render, screen } from '@testing-library/react';
import { AppProviders } from '../../app/AppProviders';
import { DashboardPage } from './DashboardPage';

describe('DashboardPage', () => {
  it('renders system foundation content', () => {
    render(
      <AppProviders>
        <DashboardPage />
      </AppProviders>,
    );

    expect(screen.getByText('Proje Altyapisi')).toBeInTheDocument();
    expect(screen.getByText('Sistem temeli')).toBeInTheDocument();
  });
});

