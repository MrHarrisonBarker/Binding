import { TestBed } from '@angular/core/testing';

import { BuildingBlocksService } from './building-blocks.service';

describe('BuildingBlocksService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: BuildingBlocksService = TestBed.get(BuildingBlocksService);
    expect(service).toBeTruthy();
  });
});
