using AgriNaviApi.Application.Common;
using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.Crops;
using AgriNaviApi.Application.Responses;
using AgriNaviApi.Application.Responses.Crops;
using AgriNaviApi.Infrastructure.Persistence.Contexts;
using AgriNaviApi.Infrastructure.Persistence.Entities;
using AgriNaviApi.Shared.Enums;
using AgriNaviApi.Shared.Exceptions;
using AgriNaviApi.Shared.Resources;
using AgriNaviApi.Shared.Utilities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AgriNaviApi.Application.Services
{
    /// <summary>
    /// 作付テーブルに関するサービス処理
    /// </summary>
    public class CropService : ICropService
    {
        const string TableName = "作付";

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IForeignKeyValidator _fkValidator;

        public CropService(AppDbContext context, IMapper mapper, IForeignKeyValidator fkValidator)
        {
            _context = context;
            _mapper = mapper;
            _fkValidator = fkValidator;
        }

        /// <summary>
        /// 作付テーブルに登録する
        /// </summary>
        /// <param name="request">作付登録用リクエストデータ</param>
        /// <returns>作付登録情報</returns>
        /// <exception cref="DuplicateEntityException">作付が登録済みの場合にスローされる</exception>
        public async Task<CropCreateResponse> CreateCropAsync(
            CropCreateRequest request,
            CancellationToken cancellationToken)
        {
            // 余計な前後スペースだけ取り除く
            var trimmedInput = request.Name.Trim();
            var trimmedInputShortName = request.ShortName?.Trim();

            // 入力値をトリミングしてDBの値と比較
            var exists = await _context.Crops
                .AsNoTracking()
                .AnyAsync(c => c.Name == trimmedInput, cancellationToken);

            // 作付が既に登録されているかをチェック
            if (exists)
            {
                throw new DuplicateEntityException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.DuplicateErrorMessage),
                    args: new[] { request.Name }
                );
            }

            await _fkValidator.ValidateGroupExistsAsync(request.GroupId, cancellationToken);

            await _fkValidator.ValidateColorExistsAsync(request.ColorId, cancellationToken);

            // 余計な前後スペースを取り除いたNameでリクエストデータを更新する
            var normalizedRequest = new CropCreateRequest
            {
                Name = trimmedInput,
                ShortName = trimmedInputShortName,
                GroupId = request.GroupId,
                ColorId = request.ColorId,
                Remark = request.Remark
            };

            // 作付名はリクエストの値をマッピング
            var crop = _mapper.Map<CropEntity>(normalizedRequest);

            _context.Crops.Add(crop);

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex) when (DbExceptionHelper.IsUniqueConstraintViolation(ex))
            {
                throw new DuplicateEntityException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.DuplicateErrorMessage),
                    args: new[] { request.Name }
                );
            }

            return _mapper.Map<CropCreateResponse>(crop);
        }

        /// <summary>
        /// 作付テーブル詳細を取得する
        /// </summary>
        /// <param name="id">作付ID</param>
        /// <returns>作付詳細情報</returns>
        /// <exception cref="EntityNotFoundException">作付が見つからなかった場合にスローされる</exception>
        public async Task<CropDetailResponse> GetCropByIdAsync(
            int id,
            CancellationToken cancellationToken)
        {
            var crop = await _context.Crops
                .Include(f => f.Group)
                .Include(f => f.Color)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);

            if (crop == null)
            {
                // エンティティが存在しない場合は、例外を投げる
                throw new EntityNotFoundException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                    args: new[] { TableName }
                );
            }

            // エンティティから DTO へ変換する
            return _mapper.Map<CropDetailResponse>(crop);
        }

        /// <summary>
        /// 作付テーブルを更新する
        /// </summary>
        /// <param name="id">作付ID</param>
        /// <param name="request">作付更新用リクエストデータ</param>
        /// <returns>作付更新情報</returns>
        /// <exception cref="EntityNotFoundException">更新対象の作付が見つからなかった場合にスローされる</exception>
        /// <exception cref="DuplicateEntityException">作付名が重複している場合にスローされる</exception>
        public async Task<CropUpdateResponse> UpdateCropAsync(
            int id,
            CropUpdateRequest request,
            CancellationToken cancellationToken)
        {
            // 余計な前後スペースだけ取り除く
            var trimmedInput = request.Name.Trim();

            // 入力値をトリミングしてDBの値と比較し、更新対象を検索
            var existingCrop = await _context.Crops
                .AsNoTracking()
                .Where(c => c.Name == trimmedInput)
                .SingleOrDefaultAsync(cancellationToken);

            if (existingCrop != null && existingCrop.Id != id)
            {
                throw new DuplicateEntityException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.DuplicateErrorMessage),
                    args: new[] { request.Name }
                );
            }

            await _fkValidator.ValidateGroupExistsAsync(request.GroupId, cancellationToken);

            await _fkValidator.ValidateColorExistsAsync(request.ColorId, cancellationToken);

            var crop = await _context.Crops.FindAsync(id, cancellationToken);

            if (crop == null)
            {
                throw new EntityNotFoundException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                    args: new[] { TableName }
                );
            }

            // 作付名はリクエストの値をマッピング
            crop = _mapper.Map(request, crop);

            // マッピング後に Name はトリム済みの trimmedInput で上書きする
            crop.Name = trimmedInput;

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex) when (DbExceptionHelper.IsUniqueConstraintViolation(ex))
            {
                throw new DuplicateEntityException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.DuplicateErrorMessage),
                    args: new[] { request.Name }
                );
            }

            return _mapper.Map<CropUpdateResponse>(crop);
        }

        /// <summary>
        /// 作付テーブルから削除する
        /// </summary>
        /// <param name="id">作付ID</param>
        /// <returns>削除結果</returns>
        /// <exception cref="EntityNotFoundException">削除対象の作付が見つからなかった場合にスローされる</exception>
        public async Task<DeleteResponse> DeleteCropAsync(
            int id,
            CancellationToken cancellationToken)
        {
            var crop = await _context.Crops.FindAsync(id, cancellationToken);

            if (crop == null)
            {
                throw new EntityNotFoundException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                    args: new[] { TableName }
                );
            }

            _context.Crops.Remove(crop);
            await _context.SaveChangesAsync(cancellationToken);

            return new DeleteResponse
            {
                Id = crop.Id,
                IsDeleted = true,
                Message = string.Format(CommonMessages.DeleteMessage, crop.Name),
                DeletedAt = null!
            };
        }

        /// <summary>
        /// 作付テーブルを検索する
        /// </summary>
        /// <param name="request">作付検索用リクエストデータ</param>
        /// <returns>検索結果</returns>
        public async Task<SearchResponse<CropListItemResponse>> SearchCropAsync(
            CropSearchRequest request,
            CancellationToken cancellationToken)
        {
            // 同一トランザクション内で複数回 SELECT を実行しても、最初の時点のスナップショットを参照し続ける
            using var tx = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.RepeatableRead, cancellationToken);

            // cropsテーブルからクエリ可能なIQueryableを取得
            var query = _context.Crops
                .Include(f => f.Group)
                .Include(f => f.Color)
                .AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchName))
            {
                // 余計な前後スペースだけ取り除く
                var trimmedSearch = request.SearchName.Trim();

                switch (request.SearchMatchType)
                {
                    case SearchMatchType.Exact:
                        query = query.Where(c => c.Name == trimmedSearch);
                        break;
                    case SearchMatchType.Prefix:
                        query = query.Where(c => c.Name.StartsWith(trimmedSearch));
                        break;
                    case SearchMatchType.Suffix:
                        query = query.Where(c => c.Name.EndsWith(trimmedSearch));
                        break;
                    case SearchMatchType.Partial:
                        query = query.Where(c => c.Name.Contains(trimmedSearch));
                        break;
                    case SearchMatchType.None:
                    default:
                        // 全件検索とする
                        break;
                }
            }

            switch (request.SortBy)
            {
                case CropSortKey.Name:
                    if (request.SortDescending)
                    {
                        query = query.OrderByDescending(c => c.Name);
                    }
                    else
                    {
                        query = query.OrderBy(c => c.Name);
                    }
                    break;
                case CropSortKey.Group:
                    if (request.SortDescending)
                    {
                        query = query.OrderByDescending(c => c.Group.Name);
                    }
                    else
                    {
                        query = query.OrderBy(c => c.Group.Name);
                    }
                    break;
                default:
                    query = query.OrderBy(c => c.Id);
                    break;
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            // トランザクションは読み取り専用なのでコミットだけ
            await tx.CommitAsync(cancellationToken);

            return new SearchResponse<CropListItemResponse>
            {
                SearchItems = _mapper.Map<List<CropListItemResponse>>(items),
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}
